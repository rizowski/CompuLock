class AccountProcess < ActiveRecord::Base
  attr_accessible :account_id, :lastrun, :name

  validates :name, :presence => true

  belongs_to :account
end
