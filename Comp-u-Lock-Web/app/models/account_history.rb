class AccountHistory < ActiveRecord::Base
  attr_accessible :account_id, :domain, :title, :url, :visit_count

  validates :account_id, presence: true
  validates :domain, presence: true, uniqueness: {scope: :account_id}
  
  belongs_to :account

  def as_json options={}
    {
      id: id,
      account_id: account_id,
      title: title,
      domain: domain,
      url: url,
      visit_count: visit_count,
      created_at: created_at,
      update_at: updated_at

    }
  end
end
