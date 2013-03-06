class CreateHistories < ActiveRecord::Migration
  def change
    create_table :histories do |t|
    	t.references :computer

      	t.string :url, :limit => 750
      	t.string :title, :limit => 300
      	t.integer :visit_count

      	t.timestamps
    end
  end
end
